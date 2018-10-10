import { Task } from './Task';
import { UserInfo } from './UserInfo';

export interface DuelInfo {
    players: UserInfo[];
    startTime: Date;
    task: Task;
}

export interface DuelTime { 
    minutes: number;
    seconds: number;
}

export const initialDuelTime : DuelTime = {
    minutes: 0,
    seconds: 5
}