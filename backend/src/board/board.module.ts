import { Module } from '@nestjs/common';
import { BoardController } from './board.controller';
import { BoardService } from './board.service';
import { TypeOrmModule } from '@nestjs/typeorm';
import { Board } from 'src/db/entity/board';
import { PasswordModule } from 'src/password/password.module';
import { CodeModule } from 'src/code/code.module';

@Module({
  imports: [
    TypeOrmModule.forFeature([
      Board
    ]),
    CodeModule,
    PasswordModule
  ],
  controllers: [BoardController],
  providers: [BoardService]
})
export class BoardModule {}
