$REQUIRED_LIBRARIES = %{
  CrossSceneReference
  DOTween
  JsonDotNet
  Rewired
  TextMesh Pro
  TMP_Typewriter
  Main Menu Background
  FronkonGames
}.lines.map(&:strip).reject(&:empty?)

$REQUIRED_LIBRARIES.each do |library|
  unless Dir.exist?(File.expand_path(library, __dir__))
    puts "Missing #{library}"
  end
end
