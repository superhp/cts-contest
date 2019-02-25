import { UserInfo } from 'ClientApp/components/models/UserInfo';
import 'isomorphic-fetch';
import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Container, Header, Icon } from 'semantic-ui-react';

interface PuzzlesProps extends RouteComponentProps<any> {
    userInfo : UserInfo;
}

export class Puzzles extends React.Component<PuzzlesProps, {}> {
    public render() {
        const contents = "TODO";
        return (
            <div>
                <div className='cg-page-header'>
                    <div className='cg-page-header-overlay'>
                        <Container fluid>
                            <Header as='h1' textAlign='center' inverted>
                                <Icon name='puzzle' />
                                <Header.Content>
                                    Puzzles
                            </Header.Content>
                            </Header>
                        </Container>
                    </div>
                </div>
                <Container>
                    {contents}
                </Container>

                <div style={{ height: 10 }}></div>
            </div>
        )
    }
}

