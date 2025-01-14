import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';

import * as bcrypt from 'bcrypt';
import { CodeService } from 'src/code/code.service';
import { Member } from 'src/db/entity/member';
import { MemberEquipment } from 'src/db/entity/member-equipment';
import { DuplicatedIdException } from 'src/exception/exception';
import { SignupDto } from 'src/user/dto/signup.dto';
import { Repository } from 'typeorm';
import { UserProfileDto } from './dto/user-profile.dto';
import { UserDto } from './dto/user.dto';
import { CustomChangeDto } from './dto/custom-change.dto';
import { PasswordService } from 'src/password/password.service';

@Injectable()
export class UserService {
    constructor (
        @InjectRepository(Member)
        private readonly memberRepository: Repository<Member>,

        @InjectRepository(MemberEquipment)
        private readonly memberEquipmentRepository: Repository<MemberEquipment>,

        private readonly codeService: CodeService,
        private readonly passwordService: PasswordService,
    ) {}

    private readonly DEFAULT_EQUIPMENTS: string[] = ['SKN_0001', 'JOB_0001', 'PET_0001', 'CSK_0001'];

    async findByAccountId(username: string): Promise<Member | undefined> {
        return this.memberRepository.findOneBy({ accountId: username });
    }

    async findByDto(dto: UserDto): Promise<Member | undefined> {
        return this.findByAccountId(dto.accountId);
    }

    async signup(dto: SignupDto): Promise<void> {
        const isDuplicated = await this.memberRepository.existsBy({accountId: dto.accountId});
        if (isDuplicated) {
            throw new DuplicatedIdException();
        }

        const member: Member = await this.memberRepository.save({
            accountId: dto.accountId,
            password: await this.passwordService.hash(dto.password),
            nickname: dto.nickname,
        });

        this.setDefaultCustom(member);
    }

    async setDefaultCustom(member: Member): Promise<void> {        
        member.equipments = [];
        for (const codeId of this.DEFAULT_EQUIPMENTS) {
            const memberEquipment = new MemberEquipment();
            memberEquipment.member = member;
            memberEquipment.customCode = this.codeService.getCommonCodeEntity(codeId);
            memberEquipment.customCodeTypeId = memberEquipment.customCode.type.id;
            
            this.memberEquipmentRepository.save(memberEquipment);
        }
    }

    async getProfile(userDto: UserDto): Promise<UserProfileDto> {
        const member: Member = await this.memberRepository.findOneBy({ accountId: userDto.accountId });

        const memberEquipments: MemberEquipment[] = await this.memberEquipmentRepository.findBy({
            member: { accountId: userDto.accountId }
        });

        const customMap = new Map<string, string>();
        memberEquipments.forEach(equipment => {
            const typeId = equipment.customCodeTypeId;
            const codeId = equipment.customCode.id;

            customMap.set(typeId, codeId);
        });

        return {
            ...userDto,
            jelly: member.jelly,
            lastCustom: customMap
        };
    }

    async changeCustom(user: UserDto, dto: CustomChangeDto) {
        const member: Member = await this.findByDto(user);
        
        const memberEquipment = new MemberEquipment();
        memberEquipment.member = member;
        memberEquipment.customCode = this.codeService.getCommonCodeEntity(dto.code);
        memberEquipment.customCodeTypeId = dto.customType;

        this.memberEquipmentRepository.save(memberEquipment);
    }
}
