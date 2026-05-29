// Root application module — imports the Users feature module
import { Module } from '@nestjs/common';
import { UsersModule } from './users/users.module.js';

@Module({
  imports: [UsersModule],
})
export class AppModule {}
