import { Task } from './Task';
import { UserInfo } from './UserInfo';

export interface DuelInfo {
    players: UserInfo[];
    startTime: Date;
    task: Task;
}