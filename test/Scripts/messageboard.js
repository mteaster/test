function initiateEdit(id)
{
    $(".post-edit").remove();
    $(".post").show();

    var content = $("#post-content-" + id).text();
    var form = $('<form class="post-edit" id="post-edit-' + id + '" method="post" action="/Dashboard/EditPost/' + id + '"><fieldset><legend>Post Message Form</legend><textarea class="text-box-edit" data-val="true" id="Content" name="Content"></textarea><br><input value="Save" type="submit"><button type=button onclick="cancelEdit(' + id + ')">Cancel</button></fieldset></form>');

    $("#post-" + id).hide();
    $("#post-container-" + id).append(form);
    $("#post-edit-" + id + " textarea").val(content);
}

function cancelEdit(id)
{
    $("#post-edit-" + id).remove();
    $("#post-" + id).show();
}