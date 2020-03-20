#!/bin/bash

GROUP_ID=
curl -s -X GET -H "X-Access-Token: $GM_TOKEN" \
  "https://api.groupme.com/v3/groups/${GROUP_ID}" \
  | jq '.response.members[] | .name + ": " + .user_id'
