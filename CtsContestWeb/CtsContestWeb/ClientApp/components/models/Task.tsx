export interface Task {
    id: number;
    name: string;
    description: string;
    value: number;
    isSolved: boolean;
}

export interface Languages {
    names: NameToDisplayNameMap;
    codes: NameToCodeMap;
}

export interface Skeleton {
    language: string;
    skeleton: string;
}

export interface CompileResult {
    compiled: boolean;
    resultCorrect: boolean;
    totalInputs: number;
    failedInput: number;
    message: string;
}

interface NameToDisplayNameMap {
    [name: string]: string;
}

interface NameToCodeMap {
    [name: string]: number;
}