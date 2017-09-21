import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Card, Icon, Image } from 'semantic-ui-react';

interface PrizeCardState {
}
export class PrizeCard extends React.Component<PrizeCardProps, PrizeCardState> {

    public render() {
        return <Card>
            <Image src={this.props.picture}>
                
            </Image>
                <Card.Content>
                    <Card.Header>
                    {this.props.name}
                    </Card.Header>
                </Card.Content>
                <Card.Content extra>
                    <a>
                    <Icon name='eur' />
                    </a>
                </Card.Content>
            </Card>
    }
}
/*export interface Prize {
    id: number;
    price: number;
    quantity: number;
    name: string;
    picture: string;
}
interface PrizeCardProps {
    prize: Prize;
}*/
interface PrizeCardProps {
    price: number;
    quantity: number;
    name: string;
    picture: string;
}