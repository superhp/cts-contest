import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Card, Icon, Image, Grid, Button } from 'semantic-ui-react';

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
                <Grid columns='equal' verticalAlign="middle" centered={true}>
                    <Grid.Row>
                        <Grid.Column>
                                {this.props.price}&nbsp;x&nbsp;
                                <Icon name='money' />
                        </Grid.Column>
                        <Grid.Column>
                                {this.props.quantity} left
                            </Grid.Column>
                        <Grid.Column>
                                <Button inverted color='blue'>Buy</Button>
                            </Grid.Column>
                        </Grid.Row>
                    </Grid>
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