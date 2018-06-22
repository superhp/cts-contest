import * as React from 'react';
import {
    Container,
    Divider,
    Header,
    Icon,
    Segment,
    Dropdown,
    Loader,
    Grid,
    Responsive
} from 'semantic-ui-react';
import AceEditor from 'react-ace';
import { Task, Languages, Skeleton, CompileResult  } from '../components/models/Task';
import { CompetitionInfo } from '../components/models/CompetitionInfo';
import { languages } from '../assets/languages';

export class CompetitionTask extends React.Component<CompetitionInfo, any> {
    languageOptions: any = [];

    constructor(props: any) {
        super(props);

        this.state = {
            taskId: this.props.task.id,
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

        Object.keys(languages.names).sort().map((lang) => {
            this.languageOptions.push({ key: lang, value: lang, text: languages.names[lang] });
        })

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
    }

    calculateEditorWidth = () => {
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
                    //this.props.onIncrementBalance(task.value);
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

    getHighlighter = (name: string) => {
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

    setCodeSkeleton = (language: string) => {
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
        let language = languages.names[data.value];
        this.setCodeSkeleton(language);

        this.setState({
            mode: this.getHighlighter(data.value),
            selectedLanguage: data.value
        })
    }

    onChange = (newValue: any) => {
        let disabledButton = true;
        if (newValue.length > 0)
            disabledButton = false;

        this.setState({
            value: newValue,
            disabledButton: disabledButton
        })
    }

    render() { 

        let saveResult = this.state.saveSuccess 
            ? <p className="success-message">Successfully saved</p>
            : <p className="error-message">Save failed</p>; 


            var submitButton =
                    <div className='cg-task-submit'>
                        <button className='cg-card-button cyan' onClick={this.compileCode} disabled={this.state.disabledButton}>Submit</button>
                        <button className='cg-card-button cyan' onClick={this.saveForLater} disabled={this.state.disabledButton}>Save for later</button>
                    </div>;

        return (
            <div>
                <TaskHeader title="Great task" />
                <Divider />
                <Container fluid>
                    <Grid columns={2} relaxed>
                        <TaskDescription description={this.props.task.description}/>

                        <Grid.Column mobile={16} tablet={8} computer={8}>
                            <LanguageDropdown options={this.languageOptions} onChange={this.setMode.bind(this)} value={this.state.selectedLanguage}/>
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

                                {this.state.showSaved && <Segment>{saveResult}</Segment>}
                                {this.state.showResults && <CompileResult result={this.state.compileResult}/>}
                                
                            </Responsive>
                        </Grid.Column>
                    </Grid>
                </Container>
                <div style={{ height: 20 }}></div>
            </div>
        )

    }
}

const TaskHeader = ({title}: {title: string}) => {
    return <Container fluid>
        <div style={{ paddingTop: 20 }}>
            <Header as='h1' textAlign='left'>
                <Header.Content>
                    {title}
                </Header.Content>
            </Header>
        </div>
    </Container>;
}

const TaskDescription = ({description}: {description: string}) => {
    return <Grid.Column mobile={16} tablet={8} computer={8}>
                            <div className="cg-task-content" dangerouslySetInnerHTML={{ __html: description }}></div>
            </Grid.Column>;
}

const LanguageDropdown = ({value, options, onChange}: {value: string, options: any, onChange: any}) => {
    return <div className='cg-language'>
        <label className='cg-label'>
            Language
        </label>
        <div className='cg-dropdown' >
            <Dropdown value={value} onChange={onChange} fluid search selection options={options} />
        </div>
    </div>;
}

const CompileResult = ({result}: {result: CompileResult}) => {
    let compilerCompletionMessage = result.resultCorrect ?
        <p className="success-message">You successfully solved this task. Congratulations!</p> :
        <p className="error-message">Failed testcase No. {result.failedInput} out of {result.totalInputs}.</p>;
    let compilerErrorMessage = result.message ? <p className="error-message">Compiler message: <br/> {result.message}</p> : null;

    let compilerMessages = <div>
        {compilerCompletionMessage}
        {compilerErrorMessage}
    </div>;

    return <Segment>
        <Header as='h2'>Result</Header>
        {
            !result ? <em>Loading...</em> :
            <div>
                {!result.compiled ?
                    <p className="error-message">{result.message ? result.message : "Unspecified error occurred. Try again or come to our stand."}</p> :
                    compilerMessages
                }
            </div>
        }
    </Segment>
}