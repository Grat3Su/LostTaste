import { Column, Entity, JoinColumn, ManyToOne } from "typeorm";
import { Member } from "./member";
import { Id } from "../typeorm-utils";
import { Adventure } from "./adventure";

@Entity({ comment: '파티원' })
export class PartyMember {
    @Id('사용자 ID')
    memberId: string;

    @Id('모험 ID')
    adventureId: string;

    @Column({
        type: 'boolean',
        nullable: false,
        comment: '파티장 여부'
    })
    isCaptain: boolean;


    @ManyToOne(() => Member)
    member: Member;

    @ManyToOne(() => Adventure)
    adventure: Adventure;
}