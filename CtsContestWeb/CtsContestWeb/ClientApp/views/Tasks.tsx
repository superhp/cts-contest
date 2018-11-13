import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import 'isomorphic-fetch';
import { Accordion, Icon, Table, Container, Header, Divider, Loader } from 'semantic-ui-react';
import * as _ from 'lodash';
import { Task } from '../components/models/Task';
import { UserInfo } from 'ClientApp/components/models/UserInfo';

//import * as GA from 'react-ga';
//GA.initialize('UA-109707377-1');
interface TasksProps extends RouteComponentProps<any> {
    userInfo : UserInfo;
}
interface TasksState {
    tasks: Task[];
    loading: boolean;
}

export class Tasks extends React.Component<TasksProps, TasksState> {
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
            : this.renderTasksAccordion(this.state.tasks);

        return (
            <div>
                <div className='cg-page-header'>
                    <div className='cg-page-header-overlay'>
                        <Container fluid>
                            <Header as='h1' textAlign='center' inverted>
                                <Icon name='tasks' />
                                <Header.Content>
                                    Tasks
                            </Header.Content>
                            </Header>
                        </Container>
                    </div>
                </div>
                <Container>
                    {contents}
                </Container>

                <div style={{ height: 10 }}></div>
            </div>
        )
    }

    private renderTasksAccordion(tasks: Task[]) {
        let panels = _.chain(tasks)
            .groupBy('value')
            .map((value: any, key: any) => ({
                key: key,
                title: this.createTaskTitle(key, value),
                content: this.createTasksSelectionTable(value)
            }))
            .value();
        return <Accordion className='cg-accordion' panels={panels} styled fluid />;
    }
    private createTaskTitle(key: number, tasks: Task[]){
        const taskNumber = tasks.length;
        const solvedTasks = tasks.filter(task => task.isSolved).length;

        return (
            <div className="task-title-header">
                <div className="task-title-name">{"Tasks for " + key + " coins"}</div>
                <div className="task-title-right">
                    <div className="task-title-solved-number">
                        {this.props.userInfo.isLoggedIn ? `${solvedTasks}/` : ''}
                        {taskNumber}
                    </div>
                </div>
            </div>
        )
    }

    private createTasksSelectionTable(tasks: Task[]) {
        let tableRows: any = [];
        tasks.forEach(t => {
            tableRows.push(
                <Table.Row className={t.isSolved ? 'solved' : ''} key={t.id}>
                    <Table.Cell selectable>
                        <Link to={"/tasks/" + t.id} style={{ paddingLeft: 35 }}> {t.name.toUpperCase()}</Link>
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

