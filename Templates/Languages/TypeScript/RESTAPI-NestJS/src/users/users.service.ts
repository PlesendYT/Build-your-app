// Users service — in-memory CRUD operations for the User entity
import { Injectable, NotFoundException } from '@nestjs/common';

// User entity shape
export interface User {
  id: number;
  name: string;
  email: string;
}

// DTO for creating a user
export interface CreateUserDto {
  name: string;
  email: string;
}

// DTO for partial updates to a user
export interface UpdateUserDto {
  name?: string;
  email?: string;
}

@Injectable()
export class UsersService {
  // In-memory user store seeded with sample data
  private users: User[] = [
    { id: 1, name: 'Alice', email: 'alice@example.com' },
    { id: 2, name: 'Bob',   email: 'bob@example.com' },
  ];
  private nextId = 3;

  // Return all users
  findAll(): User[] {
    return this.users;
  }

  // Find a single user by ID, or throw NotFoundException
  findOne(id: number): User {
    const user = this.users.find(u => u.id === id);
    if (!user) throw new NotFoundException('User not found');
    return user;
  }

  // Create a new user with an auto-incremented ID
  create(dto: CreateUserDto): User {
    const user: User = { id: this.nextId++, ...dto };
    this.users.push(user);
    return user;
  }

  // Update an existing user's name and/or email
  update(id: number, dto: UpdateUserDto): User {
    const user = this.findOne(id);
    if (dto.name !== undefined)  user.name = dto.name;
    if (dto.email !== undefined) user.email = dto.email;
    return user;
  }

  // Delete a user by ID
  remove(id: number): void {
    const idx = this.users.findIndex(u => u.id === id);
    if (idx === -1) throw new NotFoundException('User not found');
    this.users.splice(idx, 1);
  }
}
