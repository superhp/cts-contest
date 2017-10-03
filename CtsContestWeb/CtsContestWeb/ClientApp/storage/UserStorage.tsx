export class UserStorage{
    static onDecrement:any[];

    static onSaveUser(callback:any)
    {
        if(this.onDecrement === undefined)
            this.onDecrement = [];
        UserStorage.onDecrement.push(callback);
    }

    static saveUser(user:any){
        window.localStorage.setItem("user", JSON.stringify(user));

        if(this.onDecrement === undefined)
            this.onDecrement = [];
        this.onDecrement.forEach((element:any) => { element()});
    }
    static getUser(){
        const userData = window.localStorage.getItem("user");
        if(userData === null)
            return {isLoggedIn: false}
        return JSON.parse(userData);
    }
    static removeUser(){
        window.localStorage.removeItem("user");
    }
    static decrementBalance(cost:number){
        const userData = UserStorage.getUser();
        userData.balance = userData.balance - cost;
        UserStorage.saveUser(userData);
    }
    static incrementBalance(points:number){
        const userData = UserStorage.getUser();
        userData.balance = userData.balance + points;
        UserStorage.saveUser(userData);
    }

}