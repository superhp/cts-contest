export interface PuzzleInfo {
    title: string;
    tagName: string;
}

// TODO: think of a normal name
export interface PuzzleDto {
    id: number;
    identifier: string;
    isSolved: boolean;
    value: number;
    baseUrl: string;
}
