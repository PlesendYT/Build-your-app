// Users controller — REST endpoints for CRUD operations on users
import { Controller, Get, Post, Put, Delete, Param, Body, HttpCode } from '@nestjs/common';
import { UsersService } from './users.service.js';
import { CreateUserDto, UpdateUserDto } from './user.dto.js';

@Controller('users')
export class UsersController {
  constructor(private readonly usersService: UsersService) {}

  // GET /users — list all users
  @Get()
  findAll() {
    return this.usersService.findAll();
  }

  // GET /users/:id — get a single user
  @Get(':id')
  findOne(@Param('id') id: string) {
    return this.usersService.findOne(parseInt(id));
  }

  // POST /users — create a new user
  @Post()
  create(@Body() dto: CreateUserDto) {
    return this.usersService.create(dto);
  }

  // PUT /users/:id — update an existing user
  @Put(':id')
  update(@Param('id') id: string, @Body() dto: UpdateUserDto) {
    return this.usersService.update(parseInt(id), dto);
  }

  // DELETE /users/:id — remove a user (returns 204 No Content)
  @Delete(':id')
  @HttpCode(204)
  remove(@Param('id') id: string) {
    return this.usersService.remove(parseInt(id));
  }
}
