import * as React from 'react';
import {Container} from 'semantic-ui-react';

interface IInitDuelButtonProps {
    findOpponent: any,
    name: string
}

const InitDuelButton : React.SFC<IInitDuelButtonProps> = (props) => {
    return (
        <Container textAlign="center">
            <div>
                <button className='cg-card-button cyan' onClick={props.findOpponent} style={{"width": "15%"}}>
                    { props.name }
                </button>
            </div>
        </Container>
    )
}

export default InitDuelButton;