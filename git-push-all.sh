echo Updating Projects

now=$(date)

git add -A

git commit -am "$now"

git push origin master