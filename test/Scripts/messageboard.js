console.log("messageboard.js");

function edit(id)
{
    console.log("editing post " + id);

    var content = $("#content-" + id);
    var form = $('<form method="post" action="/Dashboard/EditPost?postId=' + id + '"><fieldset><legend>Post Message Form</legend><textarea class="text-box multi-line" data-val="true" id="Content" name="Content"></textarea><br><input value="Edit" type="submit"><button type=button onclick="cancel(' + id + ')">Cancel</button></fieldset></form>');
    
    content.hide();
    $("#post-" + id).append(form);
    $("#post-" + id + " textarea").val(content.val());
}

function cancel(id)
{
    console.log("canceling edit of post " + id);

    $("#edit-" + id).remove();
    $("#content-" + id).show();
}