import * as React from 'react';
import { Container, Header, Icon } from 'semantic-ui-react';

interface PageHeaderProps {
    title: string;
    iconName: string;
}

export const PageHeader = (props: PageHeaderProps) => (
    <div className='cg-page-header'>
        <div className='cg-page-header-overlay'>
            <Container fluid>
                <Header as='h1' textAlign='center' inverted>
                    <Icon name={props.iconName} />
                    <Header.Content>
                        {props.title}
                    </Header.Content>
                </Header>
            </Container>
        </div>
    </div>
);
