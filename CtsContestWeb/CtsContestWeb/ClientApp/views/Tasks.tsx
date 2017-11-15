import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import 'isomorphic-fetch';
import { Accordion, Icon, Table, Container, Header, Divider, Loader } from 'semantic-ui-react';
import * as _ from 'lodash';

//import * as GA from 'react-ga';
//GA.initialize('UA-109707377-1');

interface TasksState {
    tasks: Task[];
    loading: boolean;
}

export class Tasks extends React.Component<any, TasksState> {
    _mounted: boolean;
    constructor() {
        super();
        this.state = { tasks: [], loading: true };

        fetch('api/Task', {
            credentials: 'include'
        })
            .then(response => response.json() as Promise<Task[]>)
            .then(data => {
                if (this._mounted)
                    this.setState({ tasks: data, loading: false });
            });
    }
    componentWillMount() {
        //GA.pageview(window.location.pathname + window.location.search);
    }
    componentDidMount() {
        this._mounted = true;
    }
    componentWillUnmount() {
        this._mounted = false;
    }
    public render() {
        let contents = this.state.loading
            ? <Loader active inline='centered'>Loading</Loader>
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
                <Container>
                    {contents}
                </Container>

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
        return <Accordion className='cg-accordion' panels={panels} styled fluid />;
    }

    private static createTasksSelectionTable(tasks: Task[]) {
        let tableRows: any = [];
        tasks.forEach(t => {
            tableRows.push(
                <Table.Row className={t.isSolved ? 'solved' : ''} key={t.id}>
                    <Table.Cell selectable>
                        <Link to={"/tasks/" + t.id} style={{ paddingLeft: 35 }}> {t.name}</Link>
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

