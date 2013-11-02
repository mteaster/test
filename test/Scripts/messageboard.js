console.log("messageboard.js");

function edit(id)
{
    console.log("editing post " + id);

    var content = $("post-content-" + id).text();
    var form = $('<form id="post-edit-' + id + '" method="post" action="/Dashboard/EditPost?postId=' + id + '"><fieldset><legend>Post Message Form</legend><textarea class="text-box multi-line" data-val="true" id="Content" name="Content"></textarea><br><input value="Edit" type="submit"><button type=button onclick="cancel(' + id + ')">Cancel</button></fieldset></form>');

    $("#post-" + id).hide();
    $("#post-container-" + id).append(form);
    $("#post-edit-" + id + " textarea").val(content);
}

function cancel(id)
{
    console.log("canceling edit of post " + id);

    $("#post-edit-" + id).remove();
    $("#post-" + id).show();
}