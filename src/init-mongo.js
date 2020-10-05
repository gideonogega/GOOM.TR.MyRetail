db.createUser({
    user: "goom",
    pwd: "candy",
    roles: [
    {
       role: "readWrite",
       db: "MyRetailDb"      
    }]
})