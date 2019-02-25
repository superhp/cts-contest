import { UserInfo } from 'ClientApp/components/models/UserInfo';
import 'isomorphic-fetch';
import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Container } from 'semantic-ui-react';
import { PageHeader } from '../components/PageHeader';

interface PuzzlesProps extends RouteComponentProps<any> {
    userInfo : UserInfo;
}

export class Puzzles extends React.Component<PuzzlesProps, {}> {
    public render() {
        const contents = "TODO";
        return (
            <div>
                <PageHeader title="Puzzles" iconName="puzzle" />
                <Container>
                    {contents}
                </Container>

                <div style={{ height: 10 }}></div>
            </div>
        )
    }
}

