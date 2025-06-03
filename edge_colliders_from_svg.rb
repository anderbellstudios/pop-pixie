# This script updates one or more EdgeCollider2D components in a Unity scene to
# match paths in an SVG file.
#
# First, make sure your Unity scene contains a separate GameObject for each
# collider. The name of this GameObject should be something like
# 'Outside Collider', and an EdgeCollider2D should be added to it.
#
# Wrap these collider GameObjects in a Transform positioned at the top-left
# corner of the background image with a scale of 1 / (pixels per unit). This
# ensures that a point at (x, -y) in Unity corresponds to the point at (x, y)
# pixels in the SVG file.
#
# The SVG file must contain a group with ID 'Edge-Colliders'. For each
# collider, create path inside this group. The ID of the path should match the
# name of the corresponding GameObject, but with spaces replaced with hyphens,
# for example 'Outside-Collider'.
#
# If the level background is created using Pixelmator Pro, you can create the
# paths using the pen tool. Ensure that the stroke align option is set to
# center to prevent Pixelmator from outlining the path when exporting to SVG.
# The name of each path shape should match the name of the corresponding
# GameObject, and the group should be called 'Edge Colliders'; the spaces will
# be converted to hyphens automatically on export.

require 'nokogiri'
require 'yaml'

unless ARGV.length === 2
  STDERR.puts "Usage: ruby #{$PROGRAM_NAME} <path to unity scene> <path to svg file>"
  exit 2
end

scene_path, svg_path = ARGV

svg = Nokogiri::XML(File.open(svg_path))

edge_colliders_container = svg.css('[id=Edge-Colliders]').first

if edge_colliders_container.nil?
  STDERR.puts "Expected #{svg_path} to contain an element with ID 'Edge-Colliders'"
  exit 1
end

paths = edge_colliders_container.children.select { |child| child.name == 'path' }

EdgeColliderData = Struct.new(:name, :points)

def parse_path_data data_string
  tokens = data_string.split(' ')
  points = []

  while command = tokens.shift
    expected_arguments = {
      M: 2,
      L: 2,
      C: 6,
      Z: 0,
    }[command.to_sym]

    if expected_arguments.nil?
      STDERR.puts "Got unexpected command #{command} in:\n  #{data_string}"
      exit 1
    end

    arguments = tokens.shift(expected_arguments).map { |arg| Float(arg).round }

    case command
    when 'M', 'L'
      points << arguments
    when 'C'
      points << arguments.last(2)
    when 'Z'
      points << points.first
    end
  end

  points
end

edge_collider_data = paths.map do |path|
  name = path['id'].gsub('-', ' ')
  points = parse_path_data(path['d'])
  EdgeColliderData.new(name, points)
end

class Scene
  HEADER_REGEX = /^--- !u!\d+ &(\d+)$/

  class SceneObject
    attr_reader :id, :lines

    def initialize scene, header, id
      @scene = scene
      @header = header
      @id = id
      @lines = []
    end

    def to_s
      [@header, *@lines].join
    end

    def yaml
      @yaml ||= YAML.load(@lines.join("\n"))
    end

    def game_object? name
      yaml.has_key?('GameObject') && yaml['GameObject']['m_Name'] == name
    end

    def component_ids
      yaml['GameObject']['m_Component'].map { |entry| entry['component']['fileID'] }
    end

    def components
      component_ids.map { |id| @scene.get_by_id(id) }
    end

    def edge_collider_component
      component = components.find(&:edge_collider?)

      if component.nil?
        STDERR.puts "Expected the GameObject to have an EdgeCollider2D component:\n#{to_s}"
        exit 1
      end

      component
    end

    def edge_collider?
      yaml.has_key? 'EdgeCollider2D'
    end

    def points= points
      new_lines =
        if points.empty?
          ["  m_Points: []\n"]
        else
          ["  m_Points:\n"]
        end

      points.each do |point|
        x, y = point
        new_lines << "  - {x: #{x}, y: #{-y}}\n"
      end

      start_index = -1
      remove_count = 1

      @lines.each_with_index do |line, index|
        if start_index > -1
          if line.start_with?('  -')
            remove_count += 1
          else
            break
          end
        elsif start_index == -1 && line.start_with?('  m_Points:')
          start_index = index
        end
      end

      @lines.slice!(start_index, remove_count)
      @lines.insert(start_index, *new_lines)
      @yaml = nil
    end
  end

  def initialize path
    @path = path
    @initial_lines = []
    @objects = []

    object = nil

    File.read(path).lines.each do |line|
      header_match = HEADER_REGEX.match(line)

      if header_match
        id = header_match[1].to_i
        object = SceneObject.new(self, line, id)
        @objects << object
      elsif object
        object.lines << line
      else
        @initial_lines << line
      end
    end
  end

  def to_s
    ([@initial_lines] + @objects.map(&:to_s)).join
  end

  def save!
    File.write(@path, to_s)
  end

  def get_by_name name
    object = @objects.find { |object| object.game_object?(name) }

    if object.nil?
      STDERR.puts "Expected the scene to contain a GameObject called #{name}"
      exit 1
    end

    object
  end

  def get_by_id id
    object = @objects.find { |object| object.id == id }

    if object.nil?
      STDERR.puts "Expected the scene to contain an object with ID #{id}"
      exit 1
    end

    object
  end
end

scene = Scene.new(scene_path)

edge_collider_data.each do |data|
  component = scene.get_by_name(data.name).edge_collider_component
  component.points = data.points
end

scene.save!
