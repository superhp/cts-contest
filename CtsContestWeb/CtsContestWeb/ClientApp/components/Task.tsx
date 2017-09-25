import * as React from 'react';
import * as ReactDOM from 'react-dom';
import AceEditor from 'react-ace';
import { RouteComponentProps } from 'react-router-dom';
import { Grid, Segment, Divider, Header, Button } from 'semantic-ui-react';
import * as brace from 'brace';

import 'brace/mode/jsx';
import 'brace/theme/monokai';
import * as _ from 'lodash';

/*eslint-disable no-alert, no-console */
import 'brace/ext/language_tools';
import 'brace/ext/searchbox';
import 'brace/mode/javascript';

export class TaskComponent extends React.Component<any, any> {
    constructor(props: any){
        super(props);
        this.state = { 
            taskId: this.props.match.params.id,
            theme: 'monokai',
            mode: 'javascript',
            loadingLanguages: true,
            loadingTask: true
        };

        this.setMode = this.setMode.bind(this);
        this.onChange = this.onChange.bind(this);

        fetch('api/Task/GetLanguages')
            .then(response => response.json() as Promise<Languages>)
            .then(data => {
                var unsupportedLanguages: string[] = ['bash', 'fsharp', 'lolcode', 'smalltalk', 'whitespace', 'tsql', 'java8', 'db2', 'octave', 'racket', 'oracle'];
                unsupportedLanguages.forEach(language => {
                    delete data.codes[language];
                    delete data.names[language];
                });
                
                this.setState({ languages: data, loadingLanguages: false });

                for (let key in data.names) {
                    let compiler = this.getHighlighter(key);
                    require(`brace/mode/${compiler}`)
                }
            });
    }

    componentDidMount() {
        fetch('api/Task/' + this.state.taskId)
            .then(response => response.json() as Promise<UserInfo>)
            .then(data => {
                this.setState({ task: data, loadingTask: false });
            });
    }

    getHighlighter(name: string) {
        switch (name) {
            case 'c': 
                return 'c_cpp';
            case 'cpp': 
                return 'c_cpp';
            case 'go': 
                return 'golang';
            case 'sbcl': 
                return 'lisp';
            case 'python3': 
                return 'python';
            case 'visualbasic': 
                return 'vbscript';
            default: 
                return name;
        }
    }
     
    setMode(e: any) {
        this.setState({
            mode: this.getHighlighter(e.target.value)
        })
    }

    onChange(newValue: any) {
        this.setState({
            value: newValue
        })
    }

    private static renderLanguages(languages: Languages, setMode: any, mode: string) {
        return <select name="mode" onChange={setMode} value={mode}>
                    {Object.keys(languages.names).sort().map((lang) => <option  key={lang} value={lang}>{languages.names[lang]}</option>)}
                </select>;
    }

    private static renderTask(task: Task) {
        return <div>
            <Header as='h2'>{ task.name }</Header>
            <div className="content" dangerouslySetInnerHTML={{__html: task.description}}></div>
        </div>;
    }

    render() {
        let selectOptions = this.state.loadingLanguages
            ? <em>Loading...</em>
            : TaskComponent.renderLanguages(this.state.languages, this.setMode, this.state.mode);
        
        let task = this.state.loadingTask
            ? <em>Loading...</em>
            : TaskComponent.renderTask(this.state.task);

        return <div>
            <h1>Solve this task</h1>
            <div>
                <div className="field language-select">
                    <label>
                    Language:
                    </label>
                    <p className="control">
                        <span className="select">
                            {selectOptions}
                        </span>
                    </p>
                </div>

                <Grid columns={2} relaxed>
                    <Grid.Column>
                    <Segment basic>
                        <AceEditor 
                            mode={this.state.mode}
                            theme="monokai" 
                            name="code" 
                            fontSize={14} 
                            showPrintMargin={true} 
                            showGutter={true} 
                            highlightActiveLine={true}
                            value={this.state.value}
                            onChange={this.onChange}
                            setOptions={{
                                enableBasicAutocompletion: false,
                                enableLiveAutocompletion: true,
                                enableSnippets: false,
                                showLineNumbers: true,
                                tabSize: 2,
                            }}
                        />

                        <div><Button primary>Submit</Button></div>
                    </Segment>
                    </Grid.Column>
         
                    <Grid.Column>
                    <Segment basic>
                        {task}
                    </Segment>
                    </Grid.Column>
                </Grid>
                
                </div>
        </div>;
    
    }
}

interface NameToDisplayNameMap {
    [name: string]: string;
}

interface NameToCodeMap {
    [name: string]: number;
}

interface Languages {
    names: NameToDisplayNameMap;
    codes: NameToCodeMap;
}
