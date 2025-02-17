/**
 * ERDCloud 상에서 정의된 도메인에 따라 type, length 등을 미리 지정한 컬럼 데코레이터
 * @see https://www.erdcloud.com/d/vgwjPSzzapScQxCEb
 */
import { Column, PrimaryGeneratedColumn } from "typeorm";

export const Id = (comment?: string) => PrimaryGeneratedColumn({ type: 'bigint', comment });

export const Enum = (enumType: any, comment: string) => Column({ type: 'varchar', length: 32, enum: enumType, comment });

export const Nickname = (comment?: string) => Column({ type: 'varchar', length: 16, comment });

export const Username = (comment?: string) => Column({ type: 'varchar', length: 16, comment });

export const Password = (comment?: string) => Column({ type: 'char', length: 255, comment });

export const Jelly = (comment?: string) => Column({ type: 'int', comment });
