import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Card, Icon, Image, Grid, Button, Label } from 'semantic-ui-react';

export class PrizeCard extends React.Component<any, any> {
    constructor(props: any) {
        super(props);
    }
    openDescription = () => {
        this.props.onOpenDescription(this.props.prize);
    }
    printCategoryName(category: any) {
        if (category.toLowerCase() === 'week prize') {
            return 'Conference prize';
        }
        return category;
    }
    public render() {
        return (
            <div className='cg-card'>
                <div style={{ position: 'absolute', left: 0, top: 0, width: '100%', zIndex: 1 }}>
                    <div className='cg-title'>
                        <h2>{this.printCategoryName(this.props.prize.category)}</h2>
                    </div>
                </div>
                <div className='cg-card-image' style={{ marginTop: 71}}>
                    <img src={this.props.prize.picture} alt="" />
                </div>
                <div className='cg-card-content'>
                    <h2 className='cg-card-title'>{this.props.prize.name}</h2>
                    <div className='cg-card-actions'>
                        <div className='cg-action-item'>
                            <button className='cg-card-button cyan' onClick={this.openDescription}>Details</button>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
    renderEmpty() {
        return '';
    }
}