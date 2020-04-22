#!/bin/bash

curl -s -X GET -H "X-Access-Token: $GM_TOKEN" \
  "https://api.groupme.com/v3/groups/${GROUP_ID}" \
  | jq --arg USER_ID "${USER_ID}" \
    '.response.members[] | select(.user_id == $USER_ID)'

#  | jq '.response.members[] | .name + ": " + .user_id'
