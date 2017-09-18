import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';

interface TasksState {
    tasks: Task[];
    loading: boolean;
}

export class Tasks extends React.Component<RouteComponentProps<{}>, TasksState> {
    constructor() {
        super();
        this.state = { tasks: [], loading: true };

        fetch('api/Task')
            .then(response => response.json() as Promise<Task[]>)
            .then(data => {
                this.setState({ tasks: data, loading: false });
            });
    }

    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Tasks.renderForecastsTable(this.state.tasks);

        return <div>
            <h1>Tasks</h1>
            <p>Solve coding tasks and earn points!</p>
            {contents}
        </div>;
    }

    private static renderForecastsTable(tasks: Task[]) {
        return <table className='table'>
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Value</th>
                </tr>
            </thead>
            <tbody>
                {tasks.map(task =>
                    <tr key={task.id}>
                        <td>{task.name}</td>
                        <td>{task.value}</td>
                    </tr>
                )}
            </tbody>
        </table>;
    }
}

interface Task {
    id: number;
    name: string;
    description: string;
    value: number;
}
