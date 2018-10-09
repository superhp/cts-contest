import * as React from 'react';
import * as ReactDOM from 'react-dom';
import AceEditor from 'react-ace';
import { RouteComponentProps } from 'react-router-dom';
import { Responsive, Grid, Segment, Divider, Header, Button, Container, Loader, Dropdown } from 'semantic-ui-react';
import * as brace from 'brace';
import { Task, Languages, Skeleton, CompileResult  } from '../components/models/Task';

import 'brace/mode/jsx';
import 'brace/theme/monokai';

/*eslint-disable no-alert, no-console */
import 'brace/ext/language_tools';
import 'brace/ext/searchbox';
import 'brace/mode/java';

const MathJax: any = require("react-mathjax-preview");

export class TaskComponent extends React.Component<any, any> {
    constructor(props: any) {
        super(props);

        this.state = {
            taskId: this.props.match.params.id,
            theme: 'monokai',
            mode: 'java',
            selectedLanguage: 'java',
            loadingLanguages: true,
            loadingTask: true,
            editorWidth: this.calculateEditorWidth(),
            showResults: false,
            loadingUserInfo: true,
            disabledButton: true,
            value: "",
            showSaved: false,
            saveSuccess: true
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

                this.setState({ languages: data });

                for (let key in data.names) {
                    let compiler = this.getHighlighter(key);
                    require(`brace/mode/${compiler}`)
                }

                this.setCodeSkeleton("undefined");
            });

        this.compileCode = this.compileCode.bind(this);
        this.saveForLater = this.saveForLater.bind(this);
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
            showResults: true,
            showSaved: false,
            loadingButtons: true 
        })

        let languageCode = this.state.languages.codes[this.state.selectedLanguage];

        const formData = new FormData();
        formData.append('taskId', this.state.taskId);
        formData.append('source', this.state.value);
        formData.append('language', languageCode);
        fetch('api/Task/Solve', {
            method: 'PUT',
            body: formData,
            credentials: 'include'
        }).then(function (response) {
            if (response.ok)
                return response;

            throw new Error('Something went wrong.');
        })
            .then(response => response.json() as Promise<CompileResult>)
            .then(data => {
                let task = this.state.task;
                task.isSolved = data.resultCorrect && data.compiled;
                if (task.isSolved)
                    this.props.onIncrementBalance(task.value);
                this.setState({
                    compileResult: data,
                    task: task,
                    loadingButtons: false  
                })
            }).catch(error => this.compileError());
    }

    compileError() {
        let compileResult = {
            message: "Unexpected error occurred. Try again or come to our stand."
        }
        this.setState({
            compileResult: compileResult, 
            loadingButtons: false  
        })
    }

    saveForLater() {
        this.setState({
            showSaved: false,
            loadingButtons: true
        });

        let languageCode = this.state.languages.codes[this.state.selectedLanguage];

        const formData = new FormData();
        formData.append('taskId', this.state.taskId);
        formData.append('source', this.state.value);
        formData.append('language', languageCode);
        fetch('api/Task/SaveCode', {
            method: 'PUT',
            body: formData,
            credentials: 'include'
        })
            .then(function (response) {
                if (response.ok)
                    return response;

                throw new Error();
            })
            .then(() => {
                this.setState({
                    showSaved: true,
                    loadingButtons: false,
                    saveSuccess: true
                });
            })
            .catch(error => {
                this.setState({
                    showSaved: true,
                    loadingButtons: false,
                    saveSuccess: false
                });
            });
    }

    componentDidMount() {
        fetch('api/Task/' + this.state.taskId, {
            credentials: 'include'
        })
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
            case 'python2':
                return 'python';
            case 'visualbasic':
                return 'vbscript';
            default:
                return name;
        }
    }

    setCodeSkeleton(language: string) {
        if (language == "C#")
            language = "Csharp";
        else if (language == "C++")
            language = "Cpp";

        fetch('api/Task/GetCodeSkeleton/' + language + '/' + this.state.taskId, {
            credentials: 'include'
        })
            .then(response => response.json() as Promise<Skeleton>)
            .then(data => {
                this.onChange(data.skeleton);

                this.setState({
                    mode: this.getHighlighter(data.language),
                    selectedLanguage: data.language,
                    loadingLanguages: false
                })
            });
    }

    setMode(e: any, data: any) {
        //if (this.state.value.length == 0) {
        let language = this.state.languages.names[data.value];
        this.setCodeSkeleton(language);
        //}

        this.setState({
            mode: this.getHighlighter(data.value),
            selectedLanguage: data.value
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
        const languageOptions: any = [];

        Object.keys(languages.names).sort().map((lang) => {
            languageOptions.push({ key: lang, value: lang, text: languages.names[lang] });
        })
        return <Dropdown value={selectedLanguage} onChange={setMode} fluid search selection options={languageOptions} />
        // return <select name="mode" onChange={setMode} value={selectedLanguage}>
        //         {Object.keys(languages.names).sort().map((lang) => <option key={lang} value={lang}>{languages.names[lang]}</option>)}
        //     </select>;
    }

    private static renderTask(task: Task) {
        return <MathJax.default className="cg-task-content" math={task.description} />;
    }

    private static renderResult(compileResult: CompileResult) {        
        return <span>
            {compileResult.resultCorrect ?
                <p className="success-message">
                    You successfully solved this task. Congratulations!
                </p>
                :
                <p className="error-message">
                    Failed testcase No. {compileResult.failedInput} out of {compileResult.totalInputs}.
                </p>
            }

            {compileResult.message ?
                <p className="error-message">
                    Compiler message: <br/> {compileResult.message}
                </p>
                    :
                <span></span>
            }
        </span>;
    }

    private static renderCompileResult(compileResult: CompileResult) {
        return <div>
            {!compileResult.compiled ?
                <p className="error-message">
                    {compileResult.message ? compileResult.message : "Unspecified error occurred. Try again or come to our stand."} 
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
            ? <Loader active inline='centered'>Loading</Loader>
            : TaskComponent.renderLanguages(this.state.languages, this.setMode, this.state.selectedLanguage);

        let task = this.state.loadingTask
            ? <Loader active inline='centered'>Loading</Loader>
            : TaskComponent.renderTask(this.state.task);

        let compileResult = this.state.compileResult
            ? TaskComponent.renderCompileResult(this.state.compileResult)
            : <em>Loading...</em>;

        let saveResult = this.state.saveSuccess 
            ? <p className="success-message">Successfully saved</p>
            : <p className="error-message">Save failed</p>; 

        if (!this.state.loadingTask && this.state.task.isSolved) {
            var submitButton = this.state.showResults ? <div></div> : <div className="success-message">You successfully resolved this task.</div>;
        } else {
            var submitButton = this.props.userInfo.isLoggedIn ?
                this.state.loadingButtons
                    ? <Loader active inline='centered' />
                    :
                    <div className='cg-task-submit'>
                        <button className='cg-card-button cyan' onClick={this.compileCode} disabled={this.state.disabledButton}>Submit</button>
                        <button className='cg-card-button cyan' onClick={this.saveForLater} disabled={this.state.disabledButton}>Save for later</button>
                        {/* <Button onClick={this.compileCode} disabled={this.state.disabledButton} primary>Submit</Button>
                    <Button onClick={this.saveForLater} disabled={this.state.disabledButton} primary>Save for later</Button> */}
                    </div>
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
                            <div className='cg-language'>
                                <label className='cg-label'>
                                    Language
                                </label>
                                <div className='cg-dropdown' >
                                    {selectOptions}
                                </div>
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
                                    setOptions={{
                                        enableBasicAutocompletion: false,
                                        enableLiveAutocompletion: true,
                                        enableSnippets: false,
                                        showLineNumbers: true,
                                        tabSize: 4,
                                    }}

                                />
                                <div className='cg-padding-submit'>
                                    {submitButton}
                                </div>

                                {this.state.showSaved ?
                                    <Segment>{saveResult}</Segment>
                                    :
                                    <div></div>
                                }

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
                <div style={{ height: 20 }}></div>
            </div>
        )

    }
}

