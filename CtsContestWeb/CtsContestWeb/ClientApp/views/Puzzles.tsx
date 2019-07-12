import { UserInfo } from 'ClientApp/components/models/UserInfo';
import 'isomorphic-fetch';
import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { Container, Loader } from 'semantic-ui-react';
import { PuzzleDto } from '../components/models/Puzzle';
import { PageHeader } from '../components/PageHeader';

interface PuzzlesProps extends RouteComponentProps<any> {
    userInfo : UserInfo;
}

interface PuzzlesState {
    puzzles: PuzzleDto[];
    isLoading: boolean;
}

export class Puzzles extends React.Component<PuzzlesProps, PuzzlesState> {
    state: PuzzlesState = {
        puzzles: [],
        isLoading: true
    }

    componentDidMount() {
        fetch("/api/Puzzle")
            .then(r => r.json() as Promise<PuzzleDto[]>)
            .then(puzzles => this.setState({
                puzzles,
                isLoading: false
            }));
    }

    public render() {
        return this.state.isLoading ? this.renderLoading() : this.renderPuzzlesList();
    }

    private renderLoading() {
        return (
            <Loader active inline='centered'>Loading</Loader>
        );
    }

    private renderPuzzlesList() {
        return (
            <div>
                <PageHeader title="Puzzles" iconName="puzzle" />
                <Container>
                    <ul>
                        {this.state.puzzles.map(puzzle =>
                            <li key={puzzle.id}><Link to={`/puzzles/${puzzle.id}`}>{puzzle.identifier}</Link></li>
                        )}
                    </ul>
                </Container>

                <div style={{ height: 10 }}></div>
            </div>
        )
    }
}

