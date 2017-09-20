import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import { Accordion, Icon, Table } from 'semantic-ui-react';
import * as _ from 'lodash';

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
            : Tasks.renderTasksAccordion(this.state.tasks);

        return <div>
            <h1>Tasks</h1>
            <p>Solve coding tasks and earn points!</p>
            {contents}
        </div>;
    }

    private static renderTasksAccordion(tasks: Task[]) {
        let panels = _.chain(tasks)
            .groupBy('value')
            .map((value:any, key:any) => ({
                title: "Tasks for " + key + " points",
                content: this.createTasksSelectionTable(value)
            }))
            .value();
        return <Accordion panels={panels} styled/>;
    }

    private static createTasksSelectionTable(tasks: Task[]) {
        let tableRows: any = [];
        tasks.forEach(t => {
            tableRows.push(
                <Table.Row key={t.id}>
                    <Table.Cell selectable>
                        <a href={"/tasks/" + t.id}>{t.name}</a>
                    </Table.Cell>
                </Table.Row>);
        });
        return <Table celled>
                   <Table.Body>
                       {tableRows}
                   </Table.Body>
               </Table>;
    }
}

interface Task {
    id: number;
    name: string;
    description: string;
    value: number;
}
