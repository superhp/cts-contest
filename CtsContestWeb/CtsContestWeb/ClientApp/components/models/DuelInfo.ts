import { Task } from './Task';
import { UserInfo } from './UserInfo';

export interface DuelInfo {
    duration: number;
    players: UserInfo[];
    startTime: Date;
    task: Task;
}

export interface DuelTime { 
    minutes: number;
    seconds: number;
}