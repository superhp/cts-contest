export class UserStorage{

    static saveUser(user:any){
        window.localStorage.setItem("user", JSON.stringify(user));
    }
    static getUser(){
        const userData = window.localStorage.getItem("user");
        if(userData === null)
            return {isLoggedIn: false, balance: 0}
        return JSON.parse(userData);
    }
    static removeUser(){
        window.localStorage.removeItem("user");
    }

}