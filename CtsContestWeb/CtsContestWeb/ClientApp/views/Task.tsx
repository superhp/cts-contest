import * as React from 'react';
import * as ReactDOM from 'react-dom';
import AceEditor from 'react-ace';
import { RouteComponentProps } from 'react-router-dom';
import { Responsive, Grid, Segment, Divider, Header, Button, Container, Loader } from 'semantic-ui-react';
import * as brace from 'brace';

import 'brace/mode/jsx';
import 'brace/theme/monokai';

/*eslint-disable no-alert, no-console */
import 'brace/ext/language_tools';
import 'brace/ext/searchbox';
import 'brace/mode/javascript';

export class TaskComponent extends React.Component<any, any> {
    constructor(props: any) {
        super(props);

        this.state = {
            taskId: this.props.match.params.id,
            theme: 'monokai',
            mode: 'javascript',
            selectedLanguage: 'javascript',
            loadingLanguages: true,
            loadingTask: true,
            editorWidth: this.calculateEditorWidth(),
            showResults: false,
            loadingUserInfo: true,
            disabledButton: true
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

        this.compileCode = this.compileCode.bind(this);
    }

    calculateEditorWidth() {
        let editorWidth = (window.innerWidth - 150) / 2 + 'px';
        if (window.innerWidth < 768)
            editorWidth = (window.innerWidth - 150) + 'px';

        return editorWidth;
    }

    handleResize = () => {
        this.setState({ editorWidth: this.calculateEditorWidth() });
    }

    compileCode() {
        this.setState({
            compileResult: null,
            showResults: true
        })

        let languageCode = this.state.languages.codes[this.state.mode];

        const formData = new FormData();
        formData.append('taskId', this.state.taskId);
        formData.append('source', this.state.value);
        formData.append('language', languageCode);
        fetch('api/Task/Solve', {
            method: 'PUT',
            body: formData,
            credentials: 'include'
        })
            .then(response => response.json() as Promise<CompileResult>)
            .then(data => {
                let task = this.state.task;
                task.isSolved = data.resultCorrect && data.compiled;
                if (task.isSolved)
                    this.props.onIncrementBalance(task.value);
                this.setState({
                    compileResult: data,
                    task: task
                })
            });
    }

    componentDidMount() {
        fetch('api/Task/' + this.state.taskId)
            .then(response => response.json() as Promise<Task>)
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

    setCodeSkeleton(language: string) {
        fetch('api/Task/GetCodeSkeleton/' + language)
            .then(response => response.json() as Promise<Skeleton>)
            .then(data => {
                console.log(data);
                this.onChange(data.skeleton);
            });
    }

    setMode(e: any) {
        let language = this.state.languages.names[e.target.value];
        this.setCodeSkeleton(language);

        this.setState({
            mode: this.getHighlighter(e.target.value),
            selectedLanguage: e.target.value
        })
    }

    onChange(newValue: any) {
        let disabledButton = true;
        if (newValue.length > 0)
            disabledButton = false;

        this.setState({
            value: newValue,
            disabledButton: disabledButton
        })
    }

    private static renderLanguages(languages: Languages, setMode: any, selectedLanguage: string) {
        return <select name="mode" onChange={setMode} value={selectedLanguage}>
            {Object.keys(languages.names).sort().map((lang) => <option key={lang} value={lang}>{languages.names[lang]}</option>)}
        </select>;
    }

    private static renderTask(task: Task) {
        return <div>
            <div className="content" dangerouslySetInnerHTML={{ __html: task.description }}></div>
        </div>;
    }

    private static renderResult(compileResult: CompileResult) {
        return <span>
            {compileResult.resultCorrect ?
                <p className="success-message">
                    You successfully resolved this task. Congratulations!
                    </p>
                :
                <p className="error-message">
                    Failed {compileResult.failedInputs} out of {compileResult.totalInputs} inputs.
                    </p>
            }
        </span>;
    }

    private static renderCompileResult(compileResult: CompileResult) {
        return <div>
            {compileResult.message ?
                <p className="error-message">
                    {compileResult.message}
                </p>
                :
                this.renderResult(compileResult)
            }
        </div>;
    }
    render() {
        const taskHeaderName = this.state.loadingTask
            ? ''
            : this.state.task.name;
        let selectOptions = this.state.loadingLanguages
            ? <em>Loading...</em>
            : TaskComponent.renderLanguages(this.state.languages, this.setMode, this.state.selectedLanguage);

        let task = this.state.loadingTask
            ? <Loader active>Loading</Loader>
            : TaskComponent.renderTask(this.state.task);

        let compileResult = this.state.compileResult
            ? TaskComponent.renderCompileResult(this.state.compileResult)
            : <em>Loading...</em>;

        if (!this.state.loadingTask && this.state.task.isSolved) {
            var submitButton = this.state.showResults ? <div></div> : <div className="success-message">You successfully resolved this task.</div>;
        } else {
            var submitButton = this.props.userInfo.isLoggedIn ?
                <div><Button onClick={this.compileCode} disabled={this.state.disabledButton} primary>Submit</Button></div>
                : <div className="error-message">Please login before solving tasks</div>;
        }

        return (
            <div>
                <Container fluid>
                    <div style={{ paddingTop: 20 }}>
                        <Header as='h1' textAlign='left'>
                            <Header.Content>
                                {taskHeaderName}
                            </Header.Content>
                        </Header>
                    </div>
                </Container>

                <Divider />

                <Container fluid>

                    <Grid columns={2} relaxed>
                        <Grid.Column mobile={16} tablet={8} computer={8}>
                            {/* <Segment basic> */}
                            {task}
                            {/* </Segment> */}
                        </Grid.Column>

                        <Grid.Column mobile={16} tablet={8} computer={8}>
                            {/* <Segment basic> */}
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
                                <Responsive onUpdate={this.handleResize}>
                                    <AceEditor
                                        className='cg-editor'
                                        mode={this.state.mode}
                                        theme="monokai"
                                        name="code"
                                        fontSize={14}
                                        showPrintMargin={true}
                                        showGutter={true}
                                        highlightActiveLine={true}
                                        value={this.state.value}
                                        onChange={this.onChange}
                                        //width={this.state.editorWidth}
                                        setOptions={{
                                            enableBasicAutocompletion: false,
                                            enableLiveAutocompletion: true,
                                            enableSnippets: false,
                                            showLineNumbers: true,
                                            tabSize: 4,
                                        }}
                                        
                                    />
                                    {submitButton}

                                    {this.state.showResults ?
                                        <Segment>
                                            <Header as='h2'>Result</Header>
                                            {compileResult}
                                        </Segment>
                                        : <div></div>
                                    }
                                </Responsive>

                            {/* </Segment> */}
                        </Grid.Column>
                    </Grid>

                </Container>
                <div style={{height: 20}}></div>
            </div>
        )

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

interface Skeleton {
    language: string;
    skeleton: string;
}

interface CompileResult {
    compiled: boolean;
    resultCorrect: boolean;
    totalInputs: number;
    failedInputs: number;
    message: string;
}