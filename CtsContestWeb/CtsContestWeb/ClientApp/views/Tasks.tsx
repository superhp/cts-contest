﻿import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import 'isomorphic-fetch';
import { Accordion, Icon, Table, Container, Header, Divider, Loader } from 'semantic-ui-react';
import * as _ from 'lodash';

interface TasksState {
    tasks: Task[];
    loading: boolean;
}

export class Tasks extends React.Component<any, TasksState> {
    _mounted: boolean;
    constructor() {
        super();
        this.state = { tasks: [], loading: true };

        fetch('api/Task')
            .then(response => response.json() as Promise<Task[]>)
            .then(data => {
                if (this._mounted)
                    this.setState({ tasks: data, loading: false });
            });
    }
    componentDidMount() {
        this._mounted = true;
    }
    componentWillUnmount() {
        this._mounted = false;
    }
    public render() {
        let contents = this.state.loading
            ? <Loader active>Loading</Loader>
            : Tasks.renderTasksAccordion(this.state.tasks);

        return (
            <div>
                <div className='cg-page-header'>
                    <Container fluid>
                        <Header as='h1' textAlign='center' inverted>
                            <Icon name='tasks' />
                            <Header.Content>
                                Tasks
                        </Header.Content>
                        </Header>
                    </Container>
                </div>
                {contents}
                <div style={{ height: 10 }}></div>
            </div>
        )
    }

    private static renderTasksAccordion(tasks: Task[]) {
        let panels = _.chain(tasks)
            .groupBy('value')
            .map((value: any, key: any) => ({
                title: "Tasks for " + key + " points",
                content: this.createTasksSelectionTable(value)
            }))
            .value();
        return <Accordion className='cg-accordion' panels={panels} fluid />;
    }

    private static createTasksSelectionTable(tasks: Task[]) {
        let tableRows: any = [];
        tasks.forEach(t => {
            tableRows.push(
                <Table.Row positive={t.isSolved} key={t.id}>
                    <Table.Cell selectable>
                        <Link to={"/task/" + t.id} style={{paddingLeft: 35}}> {t.name}</Link>
                    </Table.Cell>
                </Table.Row>
            );
        });
        return (
            <Table>
                <Table.Body>
                    {tableRows}
                </Table.Body>
            </Table>
        )
    }
}

