import { UserInfo } from 'ClientApp/components/models/UserInfo';
import 'isomorphic-fetch';
import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Container, Divider, Header, Loader } from 'semantic-ui-react';
import { PuzzleInfo } from '../components/models/Puzzle';

interface PuzzleProps extends RouteComponentProps<any> {
    userInfo : UserInfo;
}

interface PuzzleState {
    title: string;
    webComponentName: string;
    isLoading: boolean;
    scripts: string[];
}

export class Puzzle extends React.Component<PuzzleProps, PuzzleState> {
    
    state: PuzzleState = {
        title: "",
        webComponentName: "",
        isLoading: true,
        scripts: []
    }

    componentDidMount() {
        const ID = `cts-puzzle-${this.props.match.params.id}`;
        const BASE_URL = `https://${ID}.azurewebsites.net`;
        this.addScript(BASE_URL+"/ui");
        fetch(BASE_URL+"/api/info")
            .then(response => response.json() as Promise<PuzzleInfo>)
            .then(result => {
                this.setState({
                    title: result.title,
                    webComponentName: result.tagName,
                    isLoading: false
                });
            });
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
                <PuzzleComponent />
            </div>
        );
    }
}

