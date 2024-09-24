var global =
{
    POST: function (url, data) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                type: 'post',
                url: url,
                data: { request: data },
                success: function (data) {
                    resolve(data);
                },
                error: function (err) {
                    reject(err);
                }
            });
        });
    },


    GET: function (url) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: url,
                dataType: 'json',
                type: 'get',
                contentType: 'application/json',
                processData: false,
                success: function (data) {
                    resolve(data);
                },
                error: function (err) {
                    reject(err);
                }
            });
        });
    },
    convertVietnameseToUnsign: function (str) {
        // Bảng chuyển đổi các ký tự có dấu thành không dấu
        const from = "àáạảãâầấậẩẫăắặẳẵèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơớờợởỡùúụủũưừứựửữỳýỵỷỹđ";
        const to = "aaaaaaaaaaaaaaaaeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyd";
        const fromArray = from.split('');
        const toArray = to.split('');

        str = str.toLowerCase();

        // Chuyển đổi các ký tự có dấu
        for (let i = 0; i < fromArray.length; i++) {
            str = str.replace(new RegExp(fromArray[i], 'g'), toArray[i]);
        }

        // Loại bỏ các ký tự đặc biệt ngoại trừ -
        str = str.replace(/[^\w\s-]/g, '');

        // Thay thế nhiều khoảng trắng thành 1 -
        str = str.replace(/\s+/g, '-');

        // Loại bỏ 2 dấu - liền nhau
        str = str.replace(/--+/g, '-');

        return str.trim();
    }
}