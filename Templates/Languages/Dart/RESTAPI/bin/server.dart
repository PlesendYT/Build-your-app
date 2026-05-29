/// Entry point for the REST API server using Shelf.
/// Serves the UserController router on localhost:8080 with request logging.
import 'package:shelf/shelf_io' as shelf_io;
import 'package:restapi/api.dart';

/// Starts the HTTP server and listens on port 8080.
Future<void> main() async {
  final controller = UserController();
  final handler = const Pipeline()
      .addMiddleware(logRequests())
      .addHandler(controller.router);

  final server = await shelf_io.serve(handler, 'localhost', 8080);
  print('REST API läuft auf http://localhost:${server.port}');
}
