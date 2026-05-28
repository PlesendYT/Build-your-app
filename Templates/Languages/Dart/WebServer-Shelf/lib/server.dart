import 'dart:io';
import 'dart:convert';
import 'package:shelf/shelf.dart';
import 'package:shelf/shelf_io.dart' as shelf_io;
import 'package:shelf_router/shelf_router.dart';

class ApiServer {
  late final Router _router;
  final List<Map<String, dynamic>> _items = [
    {'id': 1, 'name': 'Beispiel', 'value': 'Initial'}
  ];
  int _nextId = 2;

  ApiServer() {
    _router = Router()
      ..get('/', _handleIndex)
      ..get('/api/items', _handleGetItems)
      ..get('/api/items/<id>', _handleGetItem)
      ..post('/api/items', _handleCreateItem);
  }

  Middleware get middleware {
    final middlewares = Pipeline()
      .addMiddleware(logRequests())
      .addMiddleware(_corsMiddleware());
    return middlewares.addHandler(_router);
  }

  Middleware _corsMiddleware() {
    return (Handler innerHandler) {
      return (Request request) async {
        if (request.method == 'OPTIONS') {
          return Response.ok('', headers: {
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE, OPTIONS',
            'Access-Control-Allow-Headers': 'Content-Type',
          });
        }
        final response = await innerHandler(request);
        return response.change(headers: {
          'Access-Control-Allow-Origin': '*',
          ...response.headers,
        });
      };
    };
  }

  Future<Response> _handleIndex(Request request) async {
    return Response.ok(
      '<h1>Dart Shelf Server</h1>',
      headers: {'content-type': 'text/html; charset=utf-8'},
    );
  }

  Future<Response> _handleGetItems(Request request) async {
    final body = jsonEncode(_items);
    return Response.ok(body, headers: {'content-type': 'application/json'});
  }

  Future<Response> _handleGetItem(Request request, String id) async {
    final itemId = int.tryParse(id);
    if (itemId == null) {
      return Response(400, body: 'Invalid ID');
    }
    final item = _items.where((i) => i['id'] == itemId).firstOrNull;
    if (item == null) {
      return Response(404, body: 'Not found');
    }
    return Response.ok(jsonEncode(item),
        headers: {'content-type': 'application/json'});
  }

  Future<Response> _handleCreateItem(Request request) async {
    final body = await request.readAsString();
    final data = jsonDecode(body) as Map<String, dynamic>;
    final item = {
      'id': _nextId++,
      'name': data['name'],
      'value': data['value'],
    };
    _items.add(item);
    return Response(201, body: jsonEncode(item),
        headers: {'content-type': 'application/json'});
  }
}

Future<void> main() async {
  final server = ApiServer();
  final handler = server.middleware;
  final httpServer = await shelf_io.serve(handler, 'localhost', 8080);
  print('Server läuft auf http://${httpServer.address.host}:${httpServer.port}');
}
