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

import 'brace/mode/jsx';
import 'brace/theme/monokai';

/*eslint-disable no-alert, no-console */
import 'brace/ext/language_tools';
import 'brace/ext/searchbox';
import 'brace/mode/java';

interface CompetitionTaskProps {
    info: CompetitionInfo,
    submitSolution: any,
    compilerError: CompileResult | null,
    compiling: boolean
}

export class CompetitionTask extends React.Component<CompetitionTaskProps, any> {
    languageOptions: any = [];

    constructor(props: any) {
        super(props);

        this.state = {
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
            saveSuccess: true,
            languages: languages
        };

        Object.keys(languages.names).sort().map((lang) => {
            this.languageOptions.push({ key: lang, value: lang, text: languages.names[lang] });
        });
    }

    componentDidMount() {
        this.setCodeSkeleton('undefined');

        for (let key in this.state.languages.names) {
            let compiler = this.getHighlighter(key);
            require(`brace/mode/${compiler}`)
        }
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

    compileCode = () => {
        let languageCode = this.state.languages.codes[this.state.selectedLanguage];
        this.setState({loading: true});
        this.props.submitSolution(this.state.value, languageCode);
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

        fetch('api/Task/GetCodeSkeleton/' + language + '/' + this.props.info.task.id, {
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
        return (
            <div>
                <TaskHeader title={this.props.info.task.name} />
                <br/>
                <Container fluid>
                    <Grid columns={2} relaxed>
                        <TaskDescription description={this.props.info.task.description}/>

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
                                    editorProps={{
                                        $blockScrolling: Infinity
                                    }}

                                />
                                <div className='cg-padding-submit'>
                                    <div className='cg-task-submit'>
                                        <button className='cg-card-button cyan' onClick={this.compileCode} disabled={this.props.compiling}>Submit</button>
                                    </div>
                                </div>

                                {this.props.compiling && <Loader active inline='centered' />}
                                {this.props.compilerError && <CompileResult result={this.props.compilerError}/>}
                                
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