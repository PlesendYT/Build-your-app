/// REST API library providing a User model and UserController with CRUD endpoints.
import 'dart:convert';
import 'package:shelf/shelf.dart';
import 'package:shelf_router/shelf_router.dart';

/// Represents a user with id, name, email, and creation timestamp.
class User {
  final int id;
  final String name;
  final String email;
  final DateTime createdAt;

  User({required this.id, required this.name, required this.email, DateTime? createdAt})
      : createdAt = createdAt ?? DateTime.now();

  Map<String, dynamic> toJson() => {
        'id': id,
        'name': name,
        'email': email,
        'created_at': createdAt.toIso8601String(),
      };

  static User? fromJson(Map<String, dynamic> json) {
    if (json['name'] is! String || json['email'] is! String) return null;
    return User(
      id: json['id'] as int? ?? 0,
      name: json['name'] as String,
      email: json['email'] as String,
    );
  }
}

/// Handles HTTP requests for user CRUD operations via a Shelf router.
class UserController {
  final List<User> _users = [];
  int _nextId = 1;

  UserController() {
    _users.add(User(id: _nextId++, name: 'Alice', email: 'alice@example.com'));
    _users.add(User(id: _nextId++, name: 'Bob', email: 'bob@example.com'));
  }

  /// Returns a JSON array of all users.
  Future<Response> getAll(Request request) async {
    final body = jsonEncode(_users.map((u) => u.toJson()).toList());
    return Response.ok(body, headers: {'content-type': 'application/json'});
  }

  /// Returns a single user by [id] or a 404 if not found.
  Future<Response> getById(Request request, String id) async {
    final userId = int.tryParse(id);
    if (userId == null) return Response(400, body: 'Invalid ID');
    final user = _users.where((u) => u.id == userId).firstOrNull;
    if (user == null) return Response(404, body: 'User not found');
    return Response.ok(jsonEncode(user.toJson()),
        headers: {'content-type': 'application/json'});
  }

  /// Creates a new user from the JSON request body and returns it with status 201.
  Future<Response> create(Request request) async {
    final body = await request.readAsString();
    final json = jsonDecode(body) as Map<String, dynamic>;
    final user = User.fromJson(json);
    if (user == null) return Response(400, body: 'Invalid user data');
    final created = User(id: _nextId++, name: user.name, email: user.email);
    _users.add(created);
    return Response(201, body: jsonEncode(created.toJson()),
        headers: {'content-type': 'application/json'});
  }

  /// Deletes the user with the given [id]. Returns 204 on success or 404 if not found.
  Future<Response> delete(Request request, String id) async {
    final userId = int.tryParse(id);
    if (userId == null) return Response(400, body: 'Invalid ID');
    final removed = _users.removeWhere((u) => u.id == userId);
    if (removed == 0) return Response(404, body: 'User not found');
    return Response(204);
  }

  /// Builds and returns the Shelf Router with all user CRUD routes.
  Router get router {
    final r = Router();
    r.get('/api/users', getAll);
    r.get('/api/users/<id>', getById);
    r.post('/api/users', create);
    r.delete('/api/users/<id>', delete);
    return r;
  }
}
