import { UserInfo } from 'ClientApp/components/models/UserInfo';
import 'isomorphic-fetch';
import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import urljoin from "url-join";
import { Container, Divider, Header, Loader } from 'semantic-ui-react';
import { PuzzleDto, PuzzleInfo } from '../components/models/Puzzle';

interface PuzzleProps extends RouteComponentProps<any> {
    userInfo : UserInfo;
}

interface PuzzleState {
    title: string;
    webComponentName: string;
    isLoading: boolean;
    scripts: string[];
    puzzleBaseUrl: string;
}

export class Puzzle extends React.Component<PuzzleProps, PuzzleState> {
    
    state: PuzzleState = {
        title: "",
        webComponentName: "",
        isLoading: true,
        scripts: [],
        puzzleBaseUrl: ""
    }

    componentDidMount() {
        fetch(`/api/Puzzle/${this.props.match.params.id}`)
            .then(r => r.json() as Promise<PuzzleDto>)
            .then(puzzle => {
                this.addScript(urljoin(puzzle.baseUrl, "/ui"));
                this.setState({ puzzleBaseUrl: puzzle.baseUrl });
                return fetch(urljoin(puzzle.baseUrl, "/api/info"));
            })
            .then(r => r.json() as Promise<PuzzleInfo>)
            .then(result => this.setState({
                title: result.title,
                webComponentName: result.tagName,
                isLoading: false
            }));
    }

    private addScript(url: string) {
        const script = document.createElement("script");
        const loaded = document.querySelector(`script[src="${url}"`);
        if(loaded) return;

        script.src = url;
        script.async = true;
        document.body.appendChild(script);
    }

    public render() {
        const contents
            = this.state.isLoading
            ? this.renderLoader()
            : this.renderPuzzle()

        return (
            <div>
                {this.state.scripts.map(s => <script src={s} key={s}></script>)}
                <Container fluid>
                    {contents}
                </Container>
                <div style={{ height: 10 }}></div>
            </div>
        )
    }

    private renderLoader() {
        return (
            <Loader active inline='centered'>Loading</Loader>
        );
    }

    private renderPuzzle() {
        const PuzzleComponent = `${this.state.webComponentName}`;
        return (
            <div style={{paddingTop: 20}}>
                <Header as="h1">{this.state.title}</Header>
                <Divider />
                <PuzzleComponent api-base-url={this.state.puzzleBaseUrl} />
            </div>
        );
    }
}

