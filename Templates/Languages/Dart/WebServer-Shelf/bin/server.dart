/// Entry point that delegates to the library's main function.
import 'package:webserver_shelf/server.dart' as server;

/// Starts the web server by calling the library entry point.
Future<void> main() async {
  await server.main();
}
