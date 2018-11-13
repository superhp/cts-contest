import * as React from 'react';
import {Container} from 'semantic-ui-react';

interface IDuelRulesProps {
    duelState: any
}

const DuelRules : React.SFC<IDuelRulesProps> = (props) => {
    return (
        <Container>
            {!props.duelState.isInDuel ?
                <div>
                    <div className='cg-about-p'>
                        <div>Players in duel: {props.duelState.activePlayers}.</div>
                    </div>
                    <div className='cg-about-p'>
                        <div>Your statistics: {props.duelState.totalWins} wins, {props.duelState.totalLooses} loses.</div>
                    </div>
                    <div>
                        <div className='duel-how-to-container'>
                            <div className='duel-how-to'>
                                <i className='clock outline icon duel-how-to-icon'></i>
                                <div className='duel-how-to-text-container'>
                                    <div className='duel-how-to-title'>Wait</div>
                                    <div className='duel-how-to-text'>to get matched</div>
                                </div>
                            </div>
                            <div className='duel-how-to'>
                                <i className='laptop icon duel-how-to-icon'></i>
                                <div className='duel-how-to-text-container'>
                                    <div className='duel-how-to-title'>Solve</div>
                                    <div className='duel-how-to-text'>faster than opponent</div>
                                </div>
                            </div>
                            <div className='duel-how-to'>
                                <i className='trophy icon duel-how-to-icon'></i>
                                <div className='duel-how-to-text-container'>
                                    <div className='duel-how-to-title'>Win</div>
                                    <div className='duel-how-to-text'>and earn coins</div>
                                </div>
                            </div>
                        </div>

                        <div className='cg-title'>
                            <h2>Rules</h2>
                        </div>
                        <div className='cg-about-p'>
                            <ol>
                                <li><b>Opponents. </b>You can fight one opponent at a time. Opponents are matched randomly.</li>
                                <li><b>Tasks. </b>You and your opponent get the same randomly selected task. Both of you will be solving it for the first time - once you have
                                seen a particular task, it will not show up in your other duels.</li>
                                <li><b>Coins. </b>Duel tasks, depending on their difficulty, are worth 15, 20 or 40 coins. The first to solve the task wins. If nobody solves it in time,
                                no one is awarded.</li>
                                <li><b>Connection. </b>You will be able to continue the duel even if you lose connection. Once reconnected, go to the Duel page and press resume.</li>
                            </ol>
                        </div>
                    </div>
                </div>
                :
                <div></div>
            }
        </Container>
    );
}

export default DuelRules;