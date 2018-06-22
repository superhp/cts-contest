import { Task } from './Task';
import { UserInfo } from './UserInfo';

export interface CompetitionInfo {
    players: UserInfo[];
    startTime: Date;
    task: Task;
}