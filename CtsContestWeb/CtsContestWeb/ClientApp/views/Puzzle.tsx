import { UserInfo } from 'ClientApp/components/models/UserInfo';
import 'isomorphic-fetch';
import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import urljoin from "url-join";
import { Container, Divider, Header, Loader } from 'semantic-ui-react';
import { PuzzleDto, PuzzleInfo } from '../components/models/Puzzle';
import ReactDOM from 'react-dom';

interface PuzzleProps extends RouteComponentProps<any> {
    userInfo : UserInfo;
}

interface PuzzleState {
    title: string;
    webComponentName: string;
    isLoading: boolean;
    scripts: string[];
    apiCaller?: (path: string, init?: RequestInit | undefined) => Promise<Response>;
    apiBaseUrl: string;
}

export class Puzzle extends React.Component<PuzzleProps, PuzzleState> {
    
    state: PuzzleState = {
        title: "",
        webComponentName: "",
        isLoading: true,
        scripts: [],
        apiCaller: undefined,
        apiBaseUrl: ""
    }

    componentDidMount() {
        fetch(`/api/Puzzle/${this.props.match.params.id}`)
            .then(r => r.json() as Promise<PuzzleDto>)
            .then(puzzle => {
                this.addScript(urljoin(puzzle.baseUrl, "/ui"));
                //this.setState({ puzzleBaseUrl: puzzle.baseUrl });
                return Promise.all([
                    fetch(urljoin(puzzle.baseUrl, "/api/info")).then(r => r.json() as Promise<PuzzleInfo>),
                    puzzle.baseUrl
                ]);
            })
            .then(([puzzleInfo, puzzleBaseUrl]) => this.setState({
                title: puzzleInfo.title,
                webComponentName: puzzleInfo.tagName,
                isLoading: false,
                apiCaller: this.createApiCaller(puzzleBaseUrl, this.props.userInfo.provider, this.props.userInfo.accessToken),
                apiBaseUrl: puzzleBaseUrl
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
        //const elem: any = document.createElement(this.state.webComponentName);
        //elem.callApi = this.state.apiCaller;
        //console.log(elem, elem.callApi);
        return (
            <div style={{paddingTop: 20}}>
                <Header as="h1">{this.state.title}</Header>
                <Divider />
                {/*<div ref={ref=>ref && ref.appendChild(elem)}/>*/}
                <PuzzleComponent api-base-url={this.state.apiBaseUrl} />
            </div>
        );
        // TODO: passing function as attribute doesn't work (since all atrributes are strings)
        // Possible solutions:
        //   1. Find a way to pass it through properties (e.g. elem.callApi = this.state.apiCaller). Doing it simply by assignment doesn't update the child
        //      components in the PuzzleComponent.
        //   2. Pass data such as baseUrl and userInfo through attributes and build the API caller inside the puzzle component (worse way, but would work)
    }

    private createApiCaller(baseUrl: string, provider: string, accessToken: string): <T>(path: string, init?: RequestInit|undefined) => Promise<T> {
        return function<T>(path: string, init?: RequestInit | undefined) {
            if (!init) init = { headers: {} };
            return fetch(urljoin(baseUrl, path), {
                ...init,
                headers: {
                    ...init.headers,
                    "X-PROVIDER": provider,
                    "X-ACCESS-TOKEN": accessToken
                }
            })
            .then(r => {
                if (!r || !r.ok) throw Error(r.statusText);
                return r;
            })
            .then(r => r.json() as Promise<T>);
        }
    }
}

