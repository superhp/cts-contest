import * as React from 'react';

import { Button, Header, Image, Modal, Responsive } from 'semantic-ui-react'
export class PrizeDescription extends React.Component<any, any> {
    constructor(props: any) {
        super(props);
        this.state = {
            height: window.innerHeight - 5
        }
    }
    componentDidMount() {
        this.handleResize();
    }
    componentWillReceiveProps() {
        this.handleResize();
    }
    handleResize = () => {
        if (window.innerWidth < 960) {
            this.setState({ height: window.innerHeight - 25 });
        } else
            this.setState({ height: window.innerHeight - 200 });
    }
    close = () => {
        this.props.onClose();
    }
    render() {
        return (
            <Modal className='cg-modal' style={{ height: this.state.height }}
                open={this.props.open}
                closeOnEscape={true}
                closeOnRootNodeClick={true}
                onClose={this.close}
                closeIcon>
                <Modal.Header className='cg-modal-header'>{this.props.prize.name}</Modal.Header>
                <div className='cg-modal-content'>
                    <div className='cg-modal-image'>
                        <img src={this.props.prize.picture} />
                    </div>

                    <Modal.Description>
                        <div className='cg-modal-description' dangerouslySetInnerHTML={{ __html: this.props.prize.description }}>

                        </div>
                        <Responsive onUpdate={this.handleResize} />
                    </Modal.Description>
                </div>
            </Modal>
        )
    }
}