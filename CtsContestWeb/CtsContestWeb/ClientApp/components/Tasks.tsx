import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import 'isomorphic-fetch';
import { Accordion, Icon, Table, Container, Header, Divider, Loader } from 'semantic-ui-react';
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
            ? <Loader active>Loading</Loader>
            : Tasks.renderTasksAccordion(this.state.tasks);

        return <div>
            <Container fluid>
                <div style={{ marginLeft: 'auto', marginRight: 'auto', maxWidth: 225, paddingTop: 20 }}>
                    <Header as='h1' textAlign='center'>
                        <Icon name='tasks' circular />
                        <Header.Content>
                            Tasks
                            </Header.Content>
                    </Header>
                </div>
            </Container>
            <Divider />
            <Container fluid textAlign='center'>
                <p>Solve coding tasks and earn points!</p>
                <div style={{ height: 10 }} />
            </Container>
             <Container fluid>
                {contents}
                <div style={{height: 10}}></div>
            </Container>
        </div>;
    }

    private static renderTasksAccordion(tasks: Task[]) {
        let panels = _.chain(tasks)
            .groupBy('value')
            .map((value: any, key: any) => ({
                title: "Tasks for " + key + " points",
                content: this.createTasksSelectionTable(value)
            }))
            .value();
        return <Accordion panels={panels} styled fluid/>;
    }

    private static createTasksSelectionTable(tasks: Task[]) {
        let tableRows: any = [];
        tasks.forEach(t => {
            tableRows.push(
                <Table.Row positive={t.isSolved} key={t.id}>
                    <Table.Cell selectable>
                        <Link to={"/task/" + t.id}> {t.name}</Link>
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

