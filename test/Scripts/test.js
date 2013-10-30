console.log("hello world");

console.log($("#btn-test"));

$("#btn-test").click(
    function () 
    {
        $("div-test").hide();
    }
);